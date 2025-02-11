export const mockMovies = [
  {
    id: 1,
    title: "Inception",
    summary: "Film o snovima unutar snova.",
    genre: "Sci-Fi",
    release_year: 2010,
  },
  {
    id: 2,
    title: "The Matrix",
    summary: "Haker otkriva stvarnu prirodu svoje stvarnosti.",
    genre: "Action",
    release_year: 1999,
  },
  {
    id: 3,
    title: "Interstellar",
    summary:
      "Tim istraživača putuje kroz crvotočinu u potrazi za novim domom za čovječanstvo.",
    genre: "Adventure",
    release_year: 2014,
  },
];

export const mockComments = {
  1: [
    { user_id: "Alice", content: "Nevjerojatan film!", grade: 5 },
    { user_id: "Bob", content: "Vrlo zbunjujuće, ali dobro.", grade: 4 },
  ],
  2: [{ user_id: "Charlie", content: "Kultni klasik.", grade: 5 }],
  3: [
    { user_id: "Dave", content: "Vizualno zapanjujuće.", grade: 5 },
    { user_id: "Eve", content: "Predugo, ali vrijedno gledanja.", grade: 4 },
  ],
};
