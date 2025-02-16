import { getAuthToken } from "../modules/auth.js";

async function getAllUsers() {
  try {
    const response = await fetch("https://localhost:50844/api/User", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });
    if (!response.ok) {
      throw new Error(`Error fetching users: ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("An error occurred while fetching users:", error);
    return [];
  }
}

async function createUser(newUser) {
  try {
    const response = await fetch("https://localhost:50844/api/User", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(newUser),
    });
    if (!response.ok) {
      throw new Error(`Error creating user: ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("An error occurred while creating a user:", error);
    return null;
  }
}

async function loginUser(email, password) {
  try {
    const response = await fetch("http://localhost:50845/api/auth/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ Email: email, Password: password }),
    });

    if (!response.ok) {
      if (response.status === 401) {
        throw new Error("Invalid email address or password.");
      } else {
        throw new Error("An error occurred during login.");
      }
    }

    return await response.json();
  } catch (error) {
    console.error("Error logging in user:", error);
    throw error;
  }
}

async function getUserById(userId) {
  try {
    const response = await fetch(`http://localhost:50845/api/User/${userId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });
    if (!response.ok) {
      throw new Error(`Error retrieving user: ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("Error retrieving user:", error);
    throw error;
  }
}

async function registerUser(newUser) {
  try {
    const response = await fetch("http://localhost:50845/api/auth/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(newUser),
    });

    if (!response.ok) {
      const errorText = await response.text();
      let errorData = {};
      try {
        errorData = errorText ? JSON.parse(errorText) : {};
      } catch (e) {
        errorData = {};
      }
      throw new Error(errorData.message || "An error occurred during registration.");
    }

    const text = await response.text();
    return text ? JSON.parse(text) : null;
  } catch (error) {
    console.error("Error registering user:", error);
    throw error;
  }
}


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
    return null;
  }
}

async function getMovieList({ genre, year, sort } = {}) {
  let url = "http://localhost:50845/api/movie";
  const queryParams = [];

  if (genre) {
    queryParams.push(`filter=genre&value=${genre.toLowerCase()}`);
  }
  if (year) {
    queryParams.push(`filter=release_year&value=${year}`);
  }

  if (queryParams.length > 0) {
    url += `?${queryParams.join("&")}`;
  }

  if (sort) {
    const orderDirection = sort === "rating_desc" ? "desc" : "asc";
    url = `http://localhost:50845/api/movie/order?orderDirection=${orderDirection}`;
  }

  const options = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    mode: "cors",
  };

  try {
    const data = await fetchMovies(url, options);
    return data;
  } catch (error) {
    return null;
  }
}

async function createMovie(movieData) {
  const token = getAuthToken();
  if (!token) {
    throw new Error("Unauthorized: No token found.");
  }

  try {
    return await postMovie(movieData, token);
  } catch (error) {
    console.error("Error creating movie:", error);
    throw error;
  }
}

async function updateMovie(movieId, movieData) {
  const token = getAuthToken();
  if (!token) {
    throw new Error("Unauthorized: No token found.");
  }

  try {
    return await postMovie(movieData, token, movieId, "PUT");
  } catch (error) {
    console.error("Error updating movie:", error);
    throw error;
  }
}

async function deleteMovie(movieId) {
  console.log(`Deleting movie with ID: ${movieId}`);

  const token = getAuthToken();
  if (!token) {
    throw new Error("Unauthorized: No token found.");
  }

  try {
    const response = await fetch(
      `http://localhost:50845/api/movie/${movieId}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (!response.ok) {
      const errorMessage = await response.text();
      throw new Error(
        `Failed to delete movie. Server response: ${errorMessage}`
      );
    }

    console.log(`Movie with ID ${movieId} deleted successfully.`);
  } catch (error) {
    console.error("Error deleting movie:", error);
    throw error;
  }
}

async function postMovie(movieData, token, movieId = null, method = "POST") {
  let url = "http://localhost:50845/api/movie";
  if (movieId) {
    url = `http://localhost:50845/api/movie/${movieId}`;
  }

  console.log("Original movieData:", movieData);

  const enrichedMovieData = {
    id: movieId || movieData.id,
    title: movieData.title,
    summary: movieData.summary,
    genre: movieData.genre,
    releaseYear: movieData.releaseYear,
    avg_rating: movieData.avg_rating ?? 0,
    created_at: movieData.created_at || new Date().toISOString(),
    last_modified_at: new Date().toISOString(),
  };

  console.log("Enriched movieData before sending:", enrichedMovieData);
  console.log("Release Year before sending:", enrichedMovieData.release_year);

  console.log("API Request URL:", url);
  console.log("Request Method:", method);
  console.log("Authorization Token:", token);

  try {
    const response = await fetch(url, {
      method: method,
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(enrichedMovieData),
    });

    console.log("Server Response Status:", response.status);

    if (!response.ok) {
      const errorText = await response.text();
      console.error("Server Error Response:", errorText);
      throw new Error(
        `Failed to ${
          method === "DELETE" ? "delete" : "create/update"
        } movie. Server response: ${errorText}`
      );
    }

    if (response.status === 204) {
      console.log("Update successful (204 No Content)");
      return enrichedMovieData;
    }

    const responseData = await response.json();
    console.log("Server Response Data:", responseData);

    return responseData;
  } catch (error) {
    console.error(`Error in ${method} movie request:`, error);
    throw error;
  }
}

async function fetchRatings(url, options) {
  try {
    const response = await fetch(url, options);
    if (!response.ok) {
      throw new Error("Failed to fetch comments.");
    }
    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching comments:", error);
    return null;
  }
}
async function getRatingsList() {
  const url = `http://localhost:50845/api/rating`;
  const options = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    mode: "cors",
  };

  return await fetchRatings(url, options);
}

async function postComment(commentData, token) {
  try {
    const response = await fetch("http://localhost:50845/api/rating", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(commentData),
    });
    console.log(response.status)

     if (response.status === 401) {
      alert("Your session is expired, Please login again.");
      removeAuthToken();  
      throw new Error("401 Unauthorized");
    }

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Error posting comment");
    }
    return await response.json();
  } catch (error) {
    console.error("Error posting comment:", error);
    throw error;
  }
}

async function deleteComment(commentId, token) {
  try {
    const response = await fetch(`http://localhost:50845/api/rating/${commentId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    if (!response.ok) {
      const errorText = await response.text();
      let errorData = {};
      try {
        errorData = errorText ? JSON.parse(errorText) : {};
      } catch (e) {
        errorData = {};
      }
      throw new Error(errorData.message || "Error deleting comment");
    }
    return true;
  } catch (error) {
    console.error("Error deleting comment:", error);
    throw error;
  }
}

async function getAllReactions() {
  try {
    const response = await fetch("http://localhost:50845/api/ratingReactions", {
      method: "GET",
      headers: {
        "Content-Type": "application/json"
      }
    });
    if (!response.ok) {
      throw new Error(`Greška pri dohvaćanju reakcija: ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("Greška pri dohvaćanju reakcija:", error);
    return [];
  }
}

async function postReaction(reactionData, token) {
  try {
    const response = await fetch("http://localhost:50845/api/ratingReactions", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(reactionData),
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || "Error posting reaction");
    }
    return await response.json();
  } catch (error) {
    console.error("Error posting reaction:", error);
    throw error;
  }
}

async function deleteReaction(reactionId, token) {
  try {
    const response = await fetch(`https://localhost:50844/api/ratingReactions/${reactionId}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || "Error deleting reaction");
    }
    return true;
  } catch (error) {
    console.error("Error deleting reaction:", error);
    throw error;
  }
}

// Dohvaća recenzije s endpointa api/ratingComments.
// Ako želiš filtrirati (npr. po movieId), možeš proslijediti filter i value.
async function getRatingComments(filter, value) {
  let url = "http://localhost:50845/api/ratingComments";
  if (filter && value) {
    url += `?filter=${filter}&value=${value}`;
  }
  const options = {
    method: "GET",
    headers: { "Content-Type": "application/json" },
    mode: "cors",
  };

  try {
    const response = await fetch(url, options);
    if (!response.ok) {
      throw new Error(`Neuspješno dohvaćanje recenzija: ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("Greška pri dohvaćanju recenzija:", error);
    return [];
  }
}

// Šalje novu recenziju na endpoint api/ratingComments.
async function postRatingComment(ratingCommentData, token) {
  try {
    const response = await fetch("http://localhost:50845/api/ratingComments", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(ratingCommentData),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Greška pri slanju recenzije.");
    }
    return await response.json();
  } catch (error) {
    console.error("Greška pri slanju recenzije:", error);
    throw error;
  }
}



export {
  getAllUsers,
  createUser,
  loginUser,
  getUserById,
  registerUser,
  getMovieList,
  createMovie,
  updateMovie,
  deleteMovie,
  getRatingsList,
  postComment,
  deleteComment,
  getAllReactions,
  postReaction,
  deleteReaction,
  getRatingComments,
  postRatingComment
};
