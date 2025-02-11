//! ode treba stavit pravi import za fetchanje filmova I KOMENTARA
import { fetchMovies, fetchComments } from "../../ivona-test/ivona-api.js";

const displayMovieDetails = async (movieId) => {
  const movieData = (await fetchMovies()).find((m) => m.id == movieId);
  if (!movieData) {
    document.getElementById("movies-container").innerHTML =
      "<p>Data of this particular movie is not found.</p>";
    return;
  }

  const comments = await fetchComments(movieId); //! ili kakogod da se nazove funkcija za fethcanje komentara

  document.getElementById("movies-container").innerHTML = `
      <h2>${movieData.title} (${movieData.release_year})</h2>
      <p>${movieData.summary}</p>
      <h3>Comments</h3>
      <div id="comments-section">
        ${
          comments.length
            ? comments
                .map(
                  (c) =>
                    `<p><strong>${c.user_id}:</strong> ${c.content} (⭐ ${c.grade})</p>`
                )
                .join("")
            : "<p>No comments yet.</p>"
        }
      </div>
    `;
};

export { displayMovieDetails };
