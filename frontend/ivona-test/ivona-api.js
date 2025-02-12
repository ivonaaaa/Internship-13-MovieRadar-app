//test

document.addEventListener("DOMContentLoaded", async () => {
  const moviesContainer = document.getElementById("movies-container");
  async function fetchMovies(url, options) {
    try {
      const response = await fetch(url, options);
      if (!response.ok) {
        throw new Error("Failed to fetch movies.");
      }
      const data = await response.json();
      return data;
    } catch (error) {
      console.error("Error fetching movies:", error);
      return;
    }
  }

  let movies = await fetchMovies("http://localhost:5000/api/movie", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    mode: "cors",
  })
    .then((data) => {
      console.log(data);
      return data;
    })
    .catch((error) => {
      console.error("Error;", error);
      return [];
    });

  if (movies.length === 0) {
    moviesContainer.innerHTML = "<p>No movies.</p>";
    return;
  }

  movies.forEach((movie) => {
    const movieElement = document.createElement("div");
    movieElement.classList.add("movie-item");
    movieElement.innerHTML = `
            <h3>${movie.title} (${movie.release_year})</h3>
            <p>${movie.summary}</p>
            <button data-id="${movie.id}" class="view-details">Details</button>
        `;
    moviesContainer.appendChild(movieElement);
  });
});
