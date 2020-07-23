<template>
    <div class="">
        <ul>
            Products
            <br>
            <li v-for="product in products" :key="product.id">
                <span>Name: {{product.name}}, manufactured by: {{product.manufacturer}}</span>
                <br>
                <span>Description: {{product.description}}</span>
                <br>
                <span>Price: {{product.price}}</span>
                <br>
                <span>{{product.stock}} in stock.</span>
                <br>
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    name: "Cart",
    data(){
        return {
            products: [],
        };
    },
    mounted(){
        const ids = this.$store.state.checkoutProductIds;

        fetch("https://localhost:5001/api/product/many", {
            method: "POST",
            body: JSON.stringify(ids),
            headers: {
                "Content-Type": "application/json",
            }
        })
        .then(async (res) => {
            if (res.status === 200)
                this.products = await res.json();
        });    
    }
}
</script>

<style>

</style>