import Vue from 'vue'
import Vuex from 'vuex'
import createPersistedState from "vuex-persistedstate";

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
      checkoutProductIds: [],
  },
  mutations: {
    addProduct (state, productId) {
        // mutate state
        state.checkoutProductIds.push(productId);
    },
    removeProduct(state, productId){ // Assuming exists
        let index = 0;
        for (let i = 0; i < state.checkoutProductIds.length; i++){
            if (state.checkoutProductIds[i] === productId){
                index = i;
                break;
            }
        }

        state.checkoutProductIds.splice(index, 1);
    }
  },
  actions: {
  },
  modules: {
  },
  plugins: [
    createPersistedState(),
  ]
})
