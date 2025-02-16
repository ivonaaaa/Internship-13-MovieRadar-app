
import { getAllUsers, getRatingsList } from "../api/api.js";
import { createLogoutButton } from "./logout.js";

async function initUsersPage() {
  const logoutButton = createLogoutButton();
  document.body.prepend(logoutButton);

  const container = document.getElementById("users-container");
  container.innerHTML = "<h2>Users List</h2>";

  const users = await getAllUsers();
  const ratings = await getRatingsList();

  if (!users || users.length === 0) {
    container.innerHTML += "<p>No users found.</p>";
    return;
  }

  const table = document.createElement("table");
  table.innerHTML = `
    <thead>
      <tr>
        <th>First Name</th>
        <th>Last Name</th>
        <th>Reviews</th>
        <th>Average Rating</th>
      </tr>
    </thead>
    <tbody></tbody>
  `;

  const tbody = table.querySelector("tbody");

  users.forEach((user) => {
    const userRatings = ratings.filter((r) => r.userId === user.id);
    const commentsCount = userRatings.length;
    const averageRating =
      commentsCount > 0
        ? (userRatings.reduce((sum, r) => sum + r.grade, 0) / commentsCount).toFixed(1)
        : "N/A";

    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${user.firstName}</td>
      <td>${user.lastName ? user.lastName : ""}</td>
      <td>${commentsCount}</td>
      <td>${averageRating}</td>
    `;
    tbody.appendChild(tr);
  });

  container.appendChild(table);
}

document.addEventListener("DOMContentLoaded", initUsersPage);
