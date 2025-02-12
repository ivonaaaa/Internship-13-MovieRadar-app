//! ode treba dodat fetcheve za users

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

async function getAllUsers() {
  try {
    const response = await fetch("http://localhost:50845/api/User", {
      method: "GET",
      headers: {
        "Content-Type": "application/json"
      }
    });
    if (!response.ok) {
      throw new Error(`Error fetching users: ${response.statusText}`);
    }
    return await response.json();
  } catch (error) {
    console.error("An error occurred while fetching users:", error);
    // Ako dođe do greške, možemo vratiti prazan niz ili null
    return [];
  }
}

async function createUser(newUser) {
  try {
    const response = await fetch("http://localhost:50845/api/User", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newUser)
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

//! ode treba dodat fetcheve za comments

//! treba exportat funkcije s fetchanim podacima za users i comments i koristit ih dalje
export { 
  getMovieList,
  getAllUsers,
  createUser
};
