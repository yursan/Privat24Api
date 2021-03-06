﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Data.Repositories.Privat24
{
    public class CurrencyRateRepository : ICurrencyRateRepository
    {
        private const string _connectionString = "Server=DESKTOP-JQAJPMS;Initial Catalog=Privat24;User ID=sa;Password=*Offerta*";

        private const string _mergeSql = @"MERGE dbo.CurrencyRate AS Target
            USING ( VALUES(@BaseCurrency, @ToCurrency, @SaleRateNBU, @PurchaseRateNBU, @SaleRatePB, @PurchaseRatePB, @Date)) AS Source 
            (BaseCurrency, ToCurrency, SaleRateNBU, PurchaseRateNBU, SaleRatePB, PurchaseRatePB, [Date])
            ON Target.BaseCurrency = Source.BaseCurrency AND Target.ToCurrency = Source.ToCurrency AND Target.[Date] = Source.[Date]
            WHEN NOT MATCHED THEN
	            INSERT (BaseCurrency, ToCurrency, SaleRateNBU, PurchaseRateNBU, SaleRatePB, PurchaseRatePB, [Date], CreatedAt) 
                VALUES(@BaseCurrency, @ToCurrency, @SaleRateNBU, @PurchaseRateNBU, @SaleRatePB, @PurchaseRatePB, @Date, GETUTCDATE());";

        private const string _getLatestDateSql = "SELECT Min([Date]) as LastQueryDate FROM dbo.CurrencyRate";

        private readonly ILogger<CurrencyRateRepository> _logger;

        public CurrencyRateRepository(ILogger<CurrencyRateRepository> logger)
        {
            _logger = logger;
        }
        public async Task<DateTime?> GetLatestCurrencyRateDate()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(_getLatestDateSql, connection))
                {
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var result = reader["LastQueryDate"];
                            var dt = (result == DBNull.Value ? null : result);
                            return (DateTime?)dt;
                        }
                        return DateTime.Now;
                    }
                }
            }
        }

        public async Task<IReadOnlyList<CurrencyRateEntity>> GetCurrencyRates(DateTime? dateStart, DateTime? dateEnd)
        {
            var result = new List<CurrencyRateEntity>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetCurrencyRatesByDate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@dateStart", dateStart);
                    command.Parameters.AddWithValue("@dateEnd", dateEnd);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var currencyRate = new CurrencyRateEntity
                            {
                                Id = (long)reader["Id"],
                                BaseCurrency = reader["BaseCurrency"].ToString(),
                                ToCurrency = reader["ToCurrency"].ToString(),
                                SaleRateNBU = (decimal)reader["SaleRateNBU"],
                                PurchaseRateNBU = (decimal)reader["PurchaseRateNBU"],
                                SaleRatePB = (decimal)reader["SaleRatePB"],
                                PurchaseRatePB = (decimal)reader["PurchaseRatePB"],
                                Date = (DateTime)reader["Date"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };

                            result.Add(currencyRate);
                        }
                    }
                }
            }
            return result;
        }

        public async Task AddCurrencyRates(IReadOnlyList<CurrencyRateInsertEntity> rates)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(_mergeSql, connection))
                {
                    await connection.OpenAsync();

                    foreach (var currencyRate in rates)
                    {
                        AddInsertParameters(command, currencyRate);
                        try
                        {
                            int numberOfInsertedRows = await command.ExecuteNonQueryAsync();
                            _logger.LogDebug($"Number of inserted rows: {numberOfInsertedRows}");
                        }
                        catch (Exception e)
                        {
                            //log exception
                            _logger.LogError(e.Message);
                        }
                    }
                }
            }
        }

        private void AddInsertParameters(SqlCommand command, CurrencyRateInsertEntity currencyRate)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@baseCurrency", currencyRate.BaseCurrency);
            command.Parameters.AddWithValue("@toCurrency", currencyRate.ToCurrency);
            command.Parameters.AddWithValue("@saleRateNBU", currencyRate.SaleRateNBU);
            command.Parameters.AddWithValue("@purchaseRateNBU", currencyRate.PurchaseRateNBU);
            command.Parameters.AddWithValue("@saleRatePB", currencyRate.SaleRatePB);
            command.Parameters.AddWithValue("@purchaseRatePB", currencyRate.PurchaseRatePB);
            command.Parameters.AddWithValue("@date", currencyRate.Date.Date);
        }
    }
}