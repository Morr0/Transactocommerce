<template>
    <div class="">
        <ul>
            Products
            <li v-for="product in products" :key="product">
                {{product}}
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    name: "Cart",
    computed: {
        products: function (){
            const ids = this.$store.state.checkoutProductIds;
            return ids;
        }
    }
}
</script>

<style>

</style>