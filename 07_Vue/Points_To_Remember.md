# Points to Remember

## Index
1. [Script tag Sytax - Compositions vs Options](#1-script)
2. [Promise APIs](#2-promise-apis)

<br>

### 1. `<script>`

## 1. The three common approaches

A. Composition API with `<script setup>`

```vue
<script setup lang="ts">
import { ref } from 'vue'

const count = ref(0)

function increment() {
  count.value++
}
</script>
```

B. Composition API with a regular `<script>`

```vue
<script lang="ts">
import { ref } from 'vue'

export default {
  setup() {
    const count = ref(0)

    function increment() {
      count.value++
    }

    return { count, increment }
  },
}
</script>
```

C. Options API

```vue
<script lang="ts">
export default {
  data() {
    return {
      count: 0,
    }
  },
  methods: {
    increment() {
      this.count++
    },
  },
}
</script>
```


| Syntax or feature                                       | API style       |
| ------------------------------------------------------- | --------------- |
| `<script setup>`                                        | Composition API |
| `setup()` inside `export default`                       | Composition API |
| `data()`, `methods`, `computed` inside `export default` | Options API     |

<br>

### 2. Promise APIs
#### `Promise.all()` — wait for all Promises
```js
const [movies, actors, genres] = await Promise.all([
  getMovies(),
  getActors(),
  getGenres()
])
```
Can hold `1`+ results

#### `Promise.allSettled()` — wait for every outcome
```js
const results = await Promise.allSettled([
  getMovies(),
  getActors(),
  getGenres()
])
```
Can hold `1 single` result

#### `Promise.race()` — first Promise to settle wins
```js
const result = await Promise.race([
  getMovies(),
  getGenres()
])
```
Can hold `1 single` result

#### `Promise.any()` — first successful Promise wins
```js
const result = await Promise.any([
  fetchFromServerA(),
  fetchFromServerB(),
  fetchFromServerC()
])
```
Can hold `1 single` successful result

#### Compare the four main methods

| Method                 | When does it settle?                  | If one request fails                   |
| ---------------------- | ------------------------------------- | -------------------------------------- |
| `Promise.all()`        | All succeed                           | Rejects on first rejection             |
| `Promise.allSettled()` | Every Promise settles                 | Returns each outcome                   |
| `Promise.race()`       | First Promise settles                 | Rejects if the first to settle rejects |
| `Promise.any()`        | First Promise fulfills, or all reject | Keeps waiting for another success      |

An easy memory trick:

- All → all must succeed.
- AllSettled → collect every outcome.
- Race → first outcome wins.
- Any → first success wins.


