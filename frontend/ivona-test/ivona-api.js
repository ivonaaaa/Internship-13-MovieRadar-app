//mockup za api.js

import { mockMovies, mockComments } from "./ivona-mock-data.js";

export async function fetchMovies() {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(mockMovies);
    }, 500);
  });
}

export async function fetchComments(movieId) {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(mockComments[movieId] || []);
    }, 500);
  });
}
