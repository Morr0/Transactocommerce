<template>
  <div class="home">
      <ul>
          <li v-for="category in categories" :key="category.id" :id="category.id" @click.prevent="getProductsOfCategory(category.id)" 
          :title="category.description">
              <a href="">{{category.name}}</a>
          </li>
      </ul>

      <br>
      Items
      <ul v-if="products">
          <li v-for="product in products" :key="product.id">
              <span>Name: {{product.name}}, manufactured by: {{product.manufacturer}}</span>
              <br>
              <span>Description: {{product.description}}</span>
              <br>
              <span>Price: {{product.price}}</span>
              <br>
              <span>{{product.stock}} in stock.</span>
              <br>
              <a v-if="product.stock > 0" href="" @click.prevent="addToStock(product.id)">Add to cart</a>
              <br>
          </li>
      </ul>
      <ul v-else>Empty Here</ul>
      
  </div>
</template>

<script>
export default {
  name: 'Home',
  components: {
  },
  data(){
      return {
          categories: [],
          products: [],
      };
  },
  methods: {
      fetchCategories: function (){
          fetch("https://localhost:5001/api/category/")
          .then(async (res) => {
              if (res.status === 200){
                  this.categories = await res.json();
              } 
          });
          
      },
      getProductsOfCategory: async function (id){
          const res = await fetch(`https://localhost:5001/api/product?categoryId=${id}`);
          this.products = await res.json();
      },
      addToStock: function (id){ // Assumes exists
          this.$store.commit("addProduct", id);
      }
  },
  mounted(){
      this.fetchCategories();
  }
}
</script>
