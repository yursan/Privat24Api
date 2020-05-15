<template>
  <div id="app">
    <header>
      <h1>{{msg}}</h1>
    </header>
    <main>
      <aside class="sidebar">
      </aside>
      <div class="content">
        <DigraphLineChart  :title="chartTitle" :data=currencyRates></DigraphLineChart>
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
      currencyRates: [],
      ratesEndpoint: 'api/v1/currencyRates',
      chartTitle: 'EUR Currency Rates',
    }
  },
  async created() {
    await this.getAllCurrencyRates();
  },

  methods: {
    async getAllCurrencyRates() {
      return axios.get(this.ratesEndpoint)
        .then(response => {
          console.log('Response:');
          console.log(response);
          this.currencyRates = response.data;
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
