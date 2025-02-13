import { getAllUsers, getMovieList, getRatingsList } from "../api/api.js";

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
    //! ode se treba implementirat mogucnost komentiranja itd ali SAMO ZA obicne korisnike
    //!
    //!
  } catch (error) {
    console.error("Error:", error);
    document.getElementById("movies-container").innerHTML =
      "<p>An error ocurred while trying to fetch movie details.</p>";
  }
};

const averageRating = (ratings) => {
  if (ratings.length === 0) return 0;
  const total = ratings.reduce((sum, rating) => sum + rating.grade, 0);
  return parseFloat((total / ratings.length).toFixed(1));
};

export { displayMovieDetails };
