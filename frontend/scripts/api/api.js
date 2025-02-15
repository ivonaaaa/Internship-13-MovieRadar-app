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
      const errorData = await response.json();
      throw new Error(
        errorData.message || "An error occurred during registration."
      );
    }
    return await response.json();
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

//! je li ovo dobro, stavila sam da se salje taj token
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

  const token = getAuthToken(); // Dohvati token
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
          Authorization: `Bearer ${token}`, // Koristi dohvaćeni token
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

//! ode se zapravo salje, ovo se poziva u one tri osnovne radnje
async function postMovie(movieData, token, movieId = null, method = "POST") {
  let url = "http://localhost:50845/api/movie";
  if (movieId) {
    url = `http://localhost:50845/api/movie/${movieId}`;
  }

  try {
    const response = await fetch(url, {
      method: method,
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      mode: "cors",
      body: method === "DELETE" ? null : JSON.stringify(movieData),
    });
    if (!response.ok) {
      throw new Error(
        `Failed to ${method === "DELETE" ? "delete" : "create/update"} movie.`
      );
    }
    return await response.json();
  } catch (error) {
    console.error(
      `Error ${method === "DELETE" ? "deleting" : "creating/updating"} movie:`,
      error
    );
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
        // Ako API očekuje Authorization header:
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(commentData),
    });
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
};
