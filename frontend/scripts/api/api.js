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

export { getMovieList, getAllUsers, createUser, getRatingsList };
