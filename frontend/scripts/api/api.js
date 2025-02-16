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

async function getMovieList() {
  const url = "http://localhost:50845/api/movie";
  const options = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    mode: "cors",
  };

  return await fetchMovies(url, options);
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


export {
  getAllUsers,
  createUser,
  loginUser,
  getUserById,
  registerUser,
  getMovieList,
  getRatingsList,
  postComment,
  deleteComment,
  getAllReactions,
  postReaction,
  deleteReaction
};
