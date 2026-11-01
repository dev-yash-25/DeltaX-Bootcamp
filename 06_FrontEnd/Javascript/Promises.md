# Promises
A Promise is a `JavaScript object` created with `new` keyword, that represents the eventual result of an asynchronous operation. It can also represent a failure.

A Promise represents the **`future result`** of an asynchronous operation.

<br>
<div align = "center">
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/b9972f88-da5b-4450-82fa-534049232d9c" />
</div>
<br>


## 1. Imagine fetching movies from your backend

Your Vue app calls your .NET API to get movies.

```js
const response = await fetch('/api/movies')
```

The request takes time. JavaScript doesn't know the result immediately.

The operation might:

- Succeed and return the movies.
- Fail because the server is unavailable.
- Fail because of a network error.

A Promise represents this operation while JavaScript waits for its outcome.

<br>


## 2. A Promise has three states

<br>
<div align = "center">
<img width="420" alt="image" src="https://github.com/user-attachments/assets/db25ac9f-1f6a-4e38-999e-0eef9144c52e" />
</div>
<br>



A Promise starts as `pending` (default) and eventually becomes either `fulfilled` or `rejected`. Once it settles, it cannot change to another state.

<br>


## 3. So what are `resolve` and `reject`?

They are functions provided to the executor function when you create a Promise manually.

```js
const myPromise = new Promise((resolve, reject) => {
  const success = true

  if (success) {
    resolve('Movies fetched successfully')
  } else {
    reject('Failed to fetch movies')
  }
})
```

Let's break this down.

```js
new Promise((resolve, reject) => {
  // Your operation
})
```

- `new Promise(...)` creates a Promise object.
- The function passed into it is called the executor function. It runs immediately.
- `resolve(value)` fulfills the Promise with a value.
- `reject(error)` rejects the Promise with a reason, usually an error.

Think of `resolve` and `reject` as the two ways to report the outcome of the operation.

One important detail: calling `resolve()` doesn't necessarily fulfill the Promise immediately if you pass another Promise or thenable to it. It adopts that object's eventual outcome.

<br>


## 4. How do we receive the result?

You can consume a Promise using `.then()` and `.catch()`.

```js
myPromise
  .then((message) => {
    console.log(message)
  })
  .catch((error) => {
    console.error(error)
  })
```

If `resolve()` is called, the `.then()` callback receives the value.

If `reject()` is called, the `.catch()` callback handles the rejection.

Notice the difference:

- `resolve` and `reject` are used to settle a Promise.
- `.then()` and `.catch()` are used to respond to its outcome.

<br>


## 5. But do we always create Promises manually?

No. This is the part that matters most for your Vue project.

Many JavaScript APIs already return Promises. For example, `fetch()` returns a Promise, so you don't need to write `new Promise()` yourself.

```js
async function getMovies() {
  const response = await fetch('/api/movies')

  if (!response.ok) {
    throw new Error('Failed to fetch movies')
  }

  return await response.json()
}
```

Here's what happens:

1. `fetch()` returns a Promise representing the HTTP request.
2. `await` pauses this async function until that Promise settles, without blocking the JavaScript thread.
3. If the request succeeds, the code continues.
4. If `response.ok` is false, we throw an error.
5. Because the function is `async`, its returned result is itself a Promise.

Also, `fetch()` does not automatically reject for HTTP errors such as `404` or `500`. That's why we check `response.ok`.

<br>


## 6. How does this connect to your TypeScript service?

Eg function :

```js
export async function getMovies(): Promise<Movie[]> {
  const response = await apiGet<ApiResponse<Movie[]>>('/movies')
  return response.data
}
```

Read it in plain English:

> `getMovies` is an asynchronous function that calls the API, waits for its response, extracts the movie array, and returns a Promise that resolves to that array.

The important distinction is that `Promise<Movie[]>` is a TypeScript type description. It tells you what the function will eventually return. It doesn't create the Promise.

### Remember these four things

| Concept       | Purpose                                                        |
| ------------- | -------------------------------------------------------------- |
| `Promise`     | Represents an eventual result                                  |
| `resolve()`   | Fulfills a Promise with a value                                |
| `reject()`    | Rejects a Promise with a reason                                |
| `async/await` | Provides a convenient way to write code that consumes Promises |


<br>

