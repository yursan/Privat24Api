<template>
    <div id="lineChart"></div>    
</template>

<script src="https://code.jquery.com/jquery-3.5.1.js"></script>

<script>
export default {
    name: 'MGLineChart',
    props: {
        title: {
            type: String,
            default: 'EUR/USD Currency Rates'
        },
        data: {
            type: Array,
            required: true
        },
        data2: {
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
          console.log(this.data);
          console.log(this.data2);
          if(this.data && this.data2) {
            this.buildLineChart();
          }
        },
        deep: true,
      },
    },

    methods: {
        buildLineChart() {
            console.log('building chart:');
            const chartData = [
                MG.convert.date(this.data, 'date'), 
                MG.convert.date(this.data2, 'date')];
            //this.setActivityData(chartData);
            console.log(chartData);

        MG.data_graphic({
            title: this.title,
            area: false,
            x_axis: true,
            y_axis: true,
            y_rug: true,
            animate_on_load: true,
            data: chartData,
            width: 1200,
            height: 350,
            target: '#chart', // the html element that the graphic is inserted in

            x_accessor: 'date',  // the key that accesses the x value
            y_accessor: 'saleRatePB', // the key that accesses the y value
            y_label: 'sales rate',
            right: 40,
            xax_count: 2,
            color: 'green',

            point_size: 4,
            active_point_on_lines: true,
            active_point_accessor: 'active',
            active_point_size: 2,
            aggregate_rollover: true,
            brush: 'xy',
            legend: ['EUR','USD'],
            legend_target: '.legend'
            /*
            mouseover: function(d, i) {
                let date = new Date(d.date);
                let y_val = (d.saleRatePB === 0 || d.saleRatePB === null) ? 'no data' : d.saleRatePB;
                //MG.select('#chart svg .mg-active-datapoint').text(date +  '   ' + y_val);
            }
            */
          });        
        }
    }
};
</script>

<style lang="scss" scoped>

</style>