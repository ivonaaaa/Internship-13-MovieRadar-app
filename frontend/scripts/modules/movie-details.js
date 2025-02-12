import { getMovieList, getRatingsList } from "../api/api.js";

const displayMovieDetails = async (movieId) => {
  try {
    const movies = await getMovieList();
    const movieData = movies.find((m) => m.id === movieId);

    if (!movieData) {
      document.getElementById("movies-container").innerHTML =
        "<p>Movie details not found.</p>";
      return;
    }

    const allRatings = await getRatingsList();
    const movieRatings = allRatings.filter(
      (rating) => rating.movieId === movieId
    );

    document.getElementById("movies-container").innerHTML = `
      <h2>${movieData.title} (${movieData.releaseYear})</h2>
      <p>${movieData.summary}</p>
      <h3>Comments</h3>
      <div id="comments-section">
        ${
          movieRatings.length
            ? movieRatings
                .map(
                  (c) =>
                    `<p><strong>${c.userId}:</strong> ${c.review} (⭐ ${c.grade})</p>`
                )
                .join("")
            : "<p>No available comments.</p>"
        }
      </div>
    `;
  } catch (error) {
    console.error("Error:", error);
    document.getElementById("movies-container").innerHTML =
      "<p>An error ocurred while trying to fetch movie details.</p>";
  }
};

export { displayMovieDetails };
