import express from "express";
import path from "path";

// Create Express server
const app = express();

// Express configuration
app.set("port", process.env.PORT || 3000);
app.set("views", path.join(__dirname, "../views"));
//app.set("view engine", "pug");
//app.use(bodyParser.json());
//app.use(bodyParser.urlencoded({ extended: true }));

app.use(express.static(path.join(__dirname, "static")));
app.use(express.static(path.join(__dirname, "../../dist")));

export default app;
