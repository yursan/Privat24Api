<template>
<div>
    <div id="graphLineChart"></div>
</div>
</template>

<script>
import Dygraph from 'dygraphs';

export default {
    name: 'DigraphLineChart',
    props: {
        title: {
            type: String,
            default: 'EUR/USD Currency Rates'
        },
        data: {
            type: Array,
            required: true
        }
    },
    data(){
        return {
 	        
        };
    },
    watch: {
      data: {
        handler() {
          if(this.data) {
            const result = this.convertInputData();
            this.buildLineChart(result);
          }
        },
        deep: true,
        }
    },
    methods: {
        convertInputData() {
            return this.data
                .map( entry =>
                { 
                    const myMap = new Map().set('date', new Date(entry.date)).set('value', entry.saleRatePB);
                    const container = Array.from(myMap.values());
                    return container;
                }); 
        },
        buildLineChart(data){
            console.log(data);
            const lineChart = new Dygraph('graphLineChart', 
              data,
              {
                labels: [ "date", "USD"],
                fillGraph: false,
              });
        }
    }
};
</script>

<style lang="scss" scoped>

</style>