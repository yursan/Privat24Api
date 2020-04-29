using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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


        public async Task<IReadOnlyList<CurrencyRateEntity>> GetCurrencyRates(DateTime date)
        {
            var result = new List<CurrencyRateEntity>;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetCurrencyRatesByDate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@date", date);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var currencyRate = new CurrencyRateEntity();
                            currencyRate.Id = (long)reader["Id"];
                            currencyRate.BaseCurrency = reader["BaseCurrency"].ToString();
                            currencyRate.ToCurrency = reader["ToCurrency"].ToString();
                            currencyRate.SaleRateNBU = (decimal)reader["SaleRateNBU"];
                            currencyRate.PurchaseRateNBU = (decimal)reader["PurchaseRateNBU"];
                            currencyRate.SaleRatePB = (decimal)reader["SaleRatePB"];
                            currencyRate.PurchaseRatePB = (decimal)reader["PurchaseRatePB"];
                            currencyRate.Date = (DateTime)reader["Date"];
                            currencyRate.CreatedAt = (DateTime)reader["CreatedAt"];

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
                var command = new SqlCommand(_mergeSql, connection);
                await connection.OpenAsync();

                foreach (var currencyRate in rates)
                {
                    AddInsertParameters(command, currencyRate);
                    try
                    {
                        int numberOfInsertedRows = await command.ExecuteNonQueryAsync();
                        //Console.log(numberOfInsertedRows);
                    }
                    catch (Exception e)
                    {
                        //log exception
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