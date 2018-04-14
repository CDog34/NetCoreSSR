Vue.use(Vuex)

// Use this to declare "renderedHTML" as a global variable.
this.renderedHTML = ''

;(function () {
  const store = new Vuex.Store({
    state: {
      platform: 'ASP.Net Core',
      count: 1
    },

    mutations: {
      increase (state) {
        state.count++
      }
    },

    getters: {
      platform (state) {
        return state.platform
      },

      count (state) {
        return state.count
      }
    }
  })

  const vm = new Vue({
    template:
    '<div class="app">' +
    '<div>'+
    '<img width="200" src="https://vuejs.org/images/logo.png">' +
    '<img width="200" src="https://ih0.redbubble.net/image.366684642.5673/flat,800x800,075,f.u1.jpg">' +
    '</div>' +
    '<h1>Vue SSR in ASP.Net Core!</h1>' +
    '<p>If you see this, that means it does work!</p>' +
    '<div>' +
    '<span>Count: {{count}}</span>' +
    '<button @click="increase" style="margin-left: 10px">Increase</button>' +
    '<div style="margin: 10px"><a href="/update">Update html cache</a></div>' +
    '</div>' +
    '</div>',

    created () {
      console.log('Wow')
    },

    methods: {
      increase () {
        this.$store.commit('increase')
      }
    },

    computed: {
      platform () {
        return this.$store.getters.platform
      },
      count () {
        return this.$store.getters.count
      }
    },

    store
  })

  renderVueComponentToString(vm, (error,  res) => {
    if (!error) {
      this.renderedHTML = res
    }
  })
})()
