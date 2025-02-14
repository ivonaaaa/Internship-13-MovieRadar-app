import { getAllUsers, getMovieList, getRatingsList, postComment, getUserById } from "../api/api.js";
import { getAuthToken, decodeToken, removeAuthToken } from "./auth.js";

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
    const movieRatings = allRatings.filter((rating) => rating.movieId === movieId);
    const users = await getAllUsers();

    document.getElementById("movies-container").innerHTML = `
      <h2>${movieData.title} (${movieData.releaseYear})</h2>
      <p>${movieData.summary}</p>
      <h3>Average grade: ${averageRating(movieRatings)} / 10</h3>
      <h3>Comments</h3>
      <div id="comments-section">
        ${
          movieRatings.length
            ? movieRatings
                .map((c) => {
                  const user = users.find((u) => u.id === c.userId);
                  const userName = user ? user.firstName : "Unknown user";
                  return `<p><strong>${userName}:</strong> ${c.review} (⭐ ${c.grade})</p>`;
                })
                .join("")
            : "<p>No available comments.</p>"
        }
      </div>
    `;

    const token = getAuthToken();
    let userId = null;
    let isLoggedIn = false; 

    if (token) {
      try {
        const decoded = decodeToken(token);
        if (decoded && decoded.sub) {
          userId = decoded.sub;
          isLoggedIn = true;
        }
      } catch (e) {
        console.error("Token decode failed:", e);
      }
    }

      const commentFormHtml = `
        <div id="comment-form-container">
          <h4>Leave a comment:</h4>
          <textarea id="comment-content" placeholder="Your comment"></textarea>
          <br>
          <input id="comment-grade" type="number" placeholder="Grade (1-10)" min="1" max="10" step="0.1">
          <br>
          <button id="submit-comment">Submit Comment</button>
        </div>
      `;
      document.getElementById("movies-container").innerHTML += commentFormHtml;

      document.getElementById("submit-comment").addEventListener("click", async () => {
        const content = document.getElementById("comment-content").value.trim();
        const grade = parseFloat(document.getElementById("comment-grade").value);
        if (!content || isNaN(grade)) {
          alert("Please enter a valid comment and grade.");
          return;
        }

        const commentData = {
          movieId: movieId,
          review: content,
          grade: grade,
          userId: userId  
        };

        try {
          const currentToken = getAuthToken();
          await postComment(commentData, currentToken);
          alert("Comment submitted successfully!");
          displayMovieDetails(movieId);
        } catch (err) {
          window.location.href = "./index.html";
        }
      });
    }
   catch (error) {
    console.error("Error:", error);
    document.getElementById("movies-container").innerHTML =
      "<p>An error occurred while trying to fetch movie details.</p>";
  }
};

const averageRating = (ratings) => {
  if (ratings.length === 0) return 0;
  const total = ratings.reduce((sum, rating) => sum + rating.grade, 0);
  return parseFloat((total / ratings.length).toFixed(1));
};

export { displayMovieDetails };
