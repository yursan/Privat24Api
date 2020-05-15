<template>
  <div id="app">
    <header>
      <h1>{{msg}}</h1>
    </header>
    <main>
      <aside class="sidebar">
      </aside>
      <div class="content">
        <DigraphLineChart  :title="chartTitle" :data=currencyRatesEUR></DigraphLineChart>
      </div>
    </main>
  </div>
</template>

<script>
import axios from 'axios'
import MGLineChart from './components/chart/MGLineChart.vue'
import DigraphLineChart from './components/chart/DigraphLineChart.vue'

export default {
  name: 'app',
  components: {
    MGLineChart,
    DigraphLineChart
  },
  data () {
    return {
      msg: 'Privat24 vue.js SPA',
      currencyRatesUSD: [],
      currencyRatesEUR: [],
      ratesEndpoint: 'api/v1/currencyRates',
      chartTitle: 'EUR Currency Rates',
    }
  },
  async created() {
    await this.getAllCurrencyRates();
    //this.buildLineChart(this.currencyRatesEUR, this.currencyRatesUSD);
  },

  methods: {
    async getAllCurrencyRates() {
      return axios.get(this.ratesEndpoint)
        .then(response => {
          console.log('Response:');
          console.log(response);
          this.currencyRatesUSD = response.data
                .filter(c=> c.currency == 'USD')
                .map(entry => ({
                  date: new Date(entry.date).getFullYear() + '-' + (new Date(entry.date).getMonth() + 1) + '-' + new Date(entry.date).getDate(),
                  saleRatePB: entry.saleRatePB,
                  saleRateNBU: entry.saleRateNBU,
           }));;
          this.currencyRatesEUR = response.data
                .filter(c=> c.currency == 'EUR')
                .map(entry => ({
                  date: new Date(entry.date).getFullYear() + '-' + (new Date(entry.date).getMonth() + 1) + '-' + new Date(entry.date).getDate(),
                  saleRatePB: entry.saleRatePB,
                  saleRateNBU: entry.saleRateNBU,
           }));;

        })
        .catch(error => {
          console.log('-----error-------');
          console.log(error);
        })
    },
  }
}
</script>

<style lang="scss">
#app {
  margin-top: 60px;
}

</style>
