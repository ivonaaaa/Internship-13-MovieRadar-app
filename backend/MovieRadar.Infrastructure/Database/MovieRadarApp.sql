CREATE TABLE users(
    id SERIAL PRIMARY KEY,
    first_name TEXT NOT NULL, 
    last_name TEXT, 
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL,
    is_admin BOOLEAN DEFAULT FALSE NOT NULL
);
ALTER TABLE users ADD constraint check_password_length check(length(password) >= 8);
ALTER TABLE users
  ADD CONSTRAINT check_first_name CHECK (first_name ~ '^[A-Za-z]+$'),
  ADD CONSTRAINT check_last_name CHECK (last_name IS NULL OR last_name ~ '^[A-Za-z]+$'),
  ADD CONSTRAINT check_email CHECK (email ~ '^[a-zA-Z]+@[a-zA-Z]{2,}\.[a-zA-Z]{2,}$');

CREATE TABLE movies(
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL UNIQUE,
    summary TEXT,
    genre TEXT,
    release_year INT,

    CONSTRAINT release_year_check CHECK (release_year <= EXTRACT(YEAR FROM CURRENT_DATE))
);
ALTER TABLE movies
    ADD COLUMN added_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN last_modified_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP;  
ALTER TABLE movies ALTER COLUMN release_year SET NOT NULL;       

CREATE TABLE ratings(
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    movie_id INT NOT NULL REFERENCES movies(id) ON DELETE CASCADE,
    review TEXT NOT NULL,
    grade DECIMAL(3, 1) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT rating_check CHECK (grade >= 1.0 AND grade <= 10.0),
    CONSTRAINT unique_user_movie UNIQUE(user_id, movie_id)
);
-- ALTER TABLE comments 
--     RENAME COLUMN rating TO grade;
-- ALTER TABLE comments 
--     RENAME TO ratings;

CREATE TABLE ratings_comments (
    id SERIAL PRIMARY KEY, 
    rating_id INT NOT NULL REFERENCES ratings(id) ON DELETE CASCADE,
    movie_id INT NOT NULL REFERENCES movies(id) ON DELETE CASCADE,
    comment TEXT
);
CREATE TABLE ratings_reactions(
  rating_id INT PRIMARY KEY NOT NULL REFERENCES ratings(id) ON DELETE CASCADE,
  likes INT DEFAULT 0,
  dislikes INT DEFAULT 0
);


CREATE OR REPLACE FUNCTION update_last_modified_at() 
RETURNS TRIGGER AS $$ 
BEGIN
  NEW.last_modified_at = CURRENT_TIMESTAMP;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;       

CREATE TRIGGER trigger_update_last_modified_at
BEFORE UPDATE ON movies
FOR EACH ROW
EXECUTE FUNCTION update_last_modified_at();  