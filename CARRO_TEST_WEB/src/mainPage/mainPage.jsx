import React, { useState, useEffect } from "react";
import "./mainPage.css";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";

function MainPage() {
  const navigate = useNavigate();
  useEffect(() => {
    const isLogin = Cookies.get("isLogin");
    if (!isLogin) {
      navigate("/login");
    }
  });

  const logout = ()=>{
    Cookies.remove("isLogin");
    window.location.reload();
  };

  return (
    <div className="main-container">
      <div className="sidebar">
        <div className="text-at-top">Carro Website</div>
        <div className="logout-button">
          <button onClick={logout}>Logout</button>
        </div>
      </div>
      <div className="content">
        <h1>Welcome to the Main Page</h1>
        <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum</p>
      </div>
    </div>
  );
}

export default MainPage;
