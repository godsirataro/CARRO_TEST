import React, { useState, useEffect } from "react";
import "./home.css";
import logo from "../asset/logo.png";
import { Link, useNavigate } from "react-router-dom";
import Cookies from "js-cookie";

function HomePage() {

  const navigate = useNavigate();
  useEffect(() => {
    const isLogin = Cookies.get("isLogin");
    if (isLogin) {
      navigate("/mainpage");
    }
  });

  return (
    <div className="home-container">
      <div className="background">
        <img src={logo} alt="Logo" />
        <header className="header white-text">PAKAWAT PAOSOMBAT</header>
        <div className="description white-text">
          <p>
            Welcome to Carro, your one-stop destination for all things related
            to cars. Whether you're a car enthusiast or looking to buy your next
            vehicle, we've got you covered.
          </p>
        </div>
        <Link to="/login">
          <button className="primary-button ">Login</button>
        </Link>
      </div>
    </div>
  );
}

export default HomePage;
