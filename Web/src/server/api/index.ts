import express from "express";
import timeseries from "./timeseries"

const router = express.Router();

router.use('/timeseries', timeseries);

export default router;