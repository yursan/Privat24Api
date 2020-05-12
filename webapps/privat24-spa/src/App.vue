<template>
  <div id="app">
    <header>
      <h1>{{msg}}</h1>
    </header>
    <main>
      <aside class="sidebar">
        <div v-for="currencyRate in currencyRates"
        v-bind="currencyRate.id">
          {{ currencyRate.currency }}
        </div>
      </aside>
      <div class="content">

      </div>
    </main>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'app',
  data () {
    return {
      msg: 'Privat24 vue.js SPA',
      currencyRates: null,
      ratesEndpoint: 'api/v1/currencyRates',
    }
  },

  created() {
    this.getAllCurrencyRates();
  },

  methods: {
    getAllCurrencyRates() {
      axios.get(this.ratesEndpoint)
        .then(response => {
          console.log('Response:');
          console.log(response);
          this.currencyRates = response.data;
        })
        .catch(error => {
          console.log('-----error-------');
          console.log(error);
        })
    }
  }
}
</script>

<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
