import { getAllUsers, getMovieList, getRatingsList, postComment, deleteComment,postReaction,deleteReaction,getAllReactions } from "../api/api.js";
import { getAuthToken, decodeToken } from "./auth.js";
import { getIsAdmin } from "./authState.js";

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
    const isAdmin = getIsAdmin();
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
    
    const filteredRatings = allRatings.filter((rating) => rating.movieId === movieId);
    const users = await getAllUsers();

    const commentsHtml = filteredRatings.length
      ? filteredRatings
          .map((c) => {
            const user = users.find((u) => u.id === c.userId);
            const userName = user ? user.firstName : "Unknown user";
            let commentHtml = `<p><strong>${userName}:</strong> ${c.review} (⭐ ${c.grade})`;
            if (isLoggedIn && Number(c.userId) === Number(currentUserId) && !isAdmin) {
              commentHtml += ` <button class="delete-comment" data-comment-id="${c.id}">Delete</button>`;
            }else if(!isAdmin) {
              commentHtml += ` <button class="like-button" data-rating-id="${c.id}"> <i class="fas fa-thumbs-up"></i></button>
                               <button class="dislike-button" data-rating-id="${c.id}"> <i class="fas fa-thumbs-down fa-flip-horizontal"></i></button>`;
            }
            commentHtml += `</p>`;
            return commentHtml;
          })
          .join("")
      : "<p>No available reviews.</p>";

    
    let userHasReview = false;
    if (isLoggedIn) {
      userHasReview = filteredRatings.some(
        (rating) => Number(rating.userId) === Number(currentUserId)
      );
    }

   
    let htmlContent = `
      <h2>${movieData.title} (${movieData.releaseYear})</h2>
      <p>${movieData.summary}</p>
      <h3>Average grade: ${averageRating(filteredRatings)} / 10</h3>
      <h3>Reviews</h3>
      <div id="comments-section">
        ${commentsHtml}
      </div>
    `;

    if (isLoggedIn && !userHasReview && !isAdmin) {
      htmlContent += `
        <div id="comment-form-container">
          <h4>Leave a review:</h4>
          <textarea id="comment-content" placeholder="Your review"></textarea>
          <br>
          <input id="comment-grade" type="number" placeholder="Grade (1-10)" min="1" max="10" step="0.1">
          <br>
          <button id="submit-comment">Submit Review</button>
        </div>
      `;
    }

    document.getElementById("movies-container").innerHTML = htmlContent;

   
    const submitCommentButton = document.getElementById("submit-comment");
    if (submitCommentButton) {
      submitCommentButton.addEventListener("click", async () => {
        const content = document.getElementById("comment-content").value.trim();
        const grade = parseFloat(document.getElementById("comment-grade").value);
        if (!content || isNaN(grade)) {
          alert("Please enter valid comment and grade.");
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
          alert("Review added successfully!");
          displayMovieDetails(movieId);
        } catch (err) {
          console.error("Error while adding review:", err);
          alert("Error while adding review");
        }
      });
    }

    const deleteButtons = document.querySelectorAll(".delete-comment");
    deleteButtons.forEach((button) => {
      button.addEventListener("click", async (e) => {
        const commentId = e.target.dataset.commentId;
        const currentToken = getAuthToken();
        try {
          await deleteComment(commentId, currentToken);
          alert("Review deleted!");
          displayMovieDetails(movieId);
        } catch (err) {
          alert("Error while deleting review " + err.message);
        }
      });
    });

    if (isLoggedIn) {
      // Like button
      const likeButtons = document.querySelectorAll(".like-button");
      likeButtons.forEach((button) => {
        button.addEventListener("click", async (e) => {
          const ratingId = Number(e.currentTarget.dataset.ratingId);
          const currentToken = getAuthToken();
          try {
            const allReactions = await getAllReactions();
            const existingReaction = allReactions.find(
              (r) => r.ratingId === ratingId && Number(r.userId) === Number(currentUserId)
            );
            if (existingReaction) {
              if (existingReaction.reaction === "like") {
                await deleteReaction(existingReaction.id, currentToken);
                alert("Like removed!");
                displayMovieDetails(movieId);
                return;
              } else {
                await deleteReaction(existingReaction.id, currentToken);
              }
            }
            const reactionData = {
              ratingId: ratingId,
              Reaction: "like",
              userId: currentUserId
            };
            await postReaction(reactionData, currentToken);
            alert("Like added!");
            displayMovieDetails(movieId);
          } catch (err) {
            console.error("Error adding reaction:", err);
            alert("Error adding reaction.");
          }
        });
      });

        const dislikeButtons = document.querySelectorAll(".dislike-button");
        dislikeButtons.forEach((button) => {
          button.addEventListener("click", async (e) => {
            const ratingId = Number(e.target.dataset.ratingId);
            const currentToken = getAuthToken();
            try {
              const allReactions = await getAllReactions();
              const existingReaction = allReactions.find(
                (r) => r.ratingId === ratingId && Number(r.userId) === Number(currentUserId)
              );
              if (existingReaction) {
                if (existingReaction.reaction === "dislike") {
                  await deleteReaction(existingReaction.id, currentToken);
                  alert("Dislike removed!");
                  displayMovieDetails(movieId);
                  return;
                } else {
                  await deleteReaction(existingReaction.id, currentToken);
                }
              }
              const reactionData = {
                ratingId: ratingId,
                Reaction: "dislike",
                userId: currentUserId
              };
              await postReaction(reactionData, currentToken);
              alert("Dislike added!");
              displayMovieDetails(movieId);
            } catch (err) {
              console.error("Error adding reaction:", err);
              alert("Error adding reaction.");
            }
          });
        });
      }
  } catch (error) {
    console.error("Error:", error);
    document.getElementById("movies-container").innerHTML =
      "<p>An error occurred while retrieving movie details.</p>";
  }
};

const averageRating = (ratings) => {
  if (ratings.length === 0) return 0;
  const total = ratings.reduce((sum, rating) => sum + rating.grade, 0);
  return parseFloat((total / ratings.length).toFixed(1));
};

export { displayMovieDetails };
