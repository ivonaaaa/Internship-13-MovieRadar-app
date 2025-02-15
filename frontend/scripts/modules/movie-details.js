import { getAllUsers, getMovieList, getRatingsList, postComment, getUserById, deleteComment } from "../api/api.js";
import { getAuthToken, decodeToken, removeAuthToken } from "./auth.js";

const displayMovieDetails = async (movieId) => {
  try {
    const movies = await getMovieList();
    const movieData = movies.find((m) => m.id === movieId);
    const idKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    if (!movieData) {
      document.getElementById("movies-container").innerHTML =
        "<p>Movie details not found.</p>";
      return;
    }

    const token = getAuthToken();
    let currentUserId = null;
    let isLoggedIn = false;
    if (token) {
      try {
        const decoded = decodeToken(token);
        if (decoded && decoded[idKey]) {
          currentUserId = decoded[idKey];
          isLoggedIn = true;
        }
      } catch (e) {
        console.error("Token decode failed:", e);
      }
    }

    const allRatings = await getRatingsList();
    // Filtriramo recenzije za taj film
    const filteredRatings = allRatings.filter((rating) => rating.movieId === movieId);
    const users = await getAllUsers();

    const commentsHtml = filteredRatings.length
      ? filteredRatings
          .map((c) => {
            const user = users.find((u) => u.id === c.userId);
            const userName = user ? user.firstName : "Unknown user";
            let commentHtml = `<p><strong>${userName}:</strong> ${c.review} (⭐ ${c.grade})`;
            if (isLoggedIn && Number(c.userId) === Number(currentUserId)) {
              commentHtml += ` <button class="delete-comment" data-comment-id="${c.id}">Delete</button>`;
            }
            commentHtml += `</p>`;
            return commentHtml;
          })
          .join("")
      : "<p>No available comments.</p>";

    // Provjeri je li korisnik već postavio recenziju
    let userHasReview = false;
    if (isLoggedIn) {
      userHasReview = filteredRatings.some(
        (rating) => Number(rating.userId) === Number(currentUserId)
      );
    }

    // Sastavi HTML sadržaj: detalji filma i recenzije
    let htmlContent = `
      <h2>${movieData.title} (${movieData.releaseYear})</h2>
      <p>${movieData.summary}</p>
      <h3>Average grade: ${averageRating(filteredRatings)} / 10</h3>
      <h3>Comments</h3>
      <div id="comments-section">
        ${commentsHtml}
      </div>
    `;

    // Ako je korisnik prijavljen i još nije postavio recenziju, prikaži formu
    if (isLoggedIn && !userHasReview) {
      htmlContent += `
        <div id="comment-form-container">
          <h4>Leave a comment:</h4>
          <textarea id="comment-content" placeholder="Your comment"></textarea>
          <br>
          <input id="comment-grade" type="number" placeholder="Grade (1-10)" min="1" max="10" step="0.1">
          <br>
          <button id="submit-comment">Submit Comment</button>
        </div>
      `;
    }

    document.getElementById("movies-container").innerHTML = htmlContent;

    // Ako postoji forma, dodaj event listener za slanje komentara
    const submitCommentButton = document.getElementById("submit-comment");
    if (submitCommentButton) {
      submitCommentButton.addEventListener("click", async () => {
        const content = document.getElementById("comment-content").value.trim();
        const grade = parseFloat(document.getElementById("comment-grade").value);
        if (!content || isNaN(grade)) {
          alert("Molim unesite valjan komentar i ocjenu.");
          return;
        }
        const commentData = {
          movieId: movieId,
          review: content,
          grade: grade,
          userId: currentUserId  
        };

        try {
          const currentToken = getAuthToken();
          await postComment(commentData, currentToken);
          alert("Komentar uspješno dodan!");
          displayMovieDetails(movieId);
        } catch (err) {
          console.error("Greška pri dodavanju komentara:", err);
          alert("Greška pri dodavanju komentara.");
        }
      });
    }

    // Dodaj event listenere za delete gumbe
    const deleteButtons = document.querySelectorAll(".delete-comment");
    deleteButtons.forEach((button) => {
      button.addEventListener("click", async (e) => {
        const commentId = e.target.dataset.commentId;
        const currentToken = getAuthToken();
        try {
          await deleteComment(commentId, currentToken);
          alert("Komentar uspješno obrisan!");
          displayMovieDetails(movieId);
        } catch (err) {
          alert("Greška pri brisanju komentara: " + err.message);
        }
      });
    });
  } catch (error) {
    console.error("Error:", error);
    document.getElementById("movies-container").innerHTML =
      "<p>Došlo je do pogreške prilikom dohvaćanja detalja filma.</p>";
  }
};

const averageRating = (ratings) => {
  if (ratings.length === 0) return 0;
  const total = ratings.reduce((sum, rating) => sum + rating.grade, 0);
  return parseFloat((total / ratings.length).toFixed(1));
};

export { displayMovieDetails };
