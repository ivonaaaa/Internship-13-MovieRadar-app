//! ZASTO NE RADIIIIiiiii

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
  const url = "http://localhost:5000/api/movie";
  const options = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    mode: "cors",
  };

  return await fetchMovies(url, options);
}

//! ode treba dodat fetcheve za comments

//! treba exportat funkcije s fetchanim podacima za users i comments i koristit ih dalje
export { getMovieList };
