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
//! FILTRIRANJE NE RADI KAKO TREBA
// async function getMovieList({ genre, year, sort } = {}) {
//   let url = "http://localhost:50845/api/movie";
//   const queryParams = [];

//   console.log("🔎 Initial URL:", url);
//   console.log("📌 Filter parameters received:", { genre, year, sort });

//   if (genre) {
//     queryParams.push(`filter=genre&parameter=${genre}`);
//     console.log("✅ Genre filter added:", `filter=genre&parameter=${genre}`);
//   }
//   if (year) {
//     queryParams.push(`filter=release_year&parameter=${year}`);
//     console.log(
//       "✅ Year filter added:",
//       `filter=release_year&parameter=${year}`
//     );
//   }
//   if (sort) {
//     const orderDirection = sort === "rating_desc" ? "desc" : "asc";
//     queryParams.push(`orderDirection=${orderDirection}`);
//     console.log("✅ Sorting added:", `orderDirection=${orderDirection}`);
//   }

//   if (queryParams.length > 0) {
//     url += `?${queryParams.join("&")}`;
//   }

//   console.log("🚀 Final URL being fetched:", url);

//   const options = {
//     method: "GET",
//     headers: {
//       "Content-Type": "application/json",
//     },
//     mode: "cors",
//   };

//   try {
//     const data = await fetchMovies(url, options);
//     console.log("🎬 Movies fetched:", data);
//     return data;
//   } catch (error) {
//     console.error("❌ Error fetching movies:", error);
//     return null;
//   }
// }

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
