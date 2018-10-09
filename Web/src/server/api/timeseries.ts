import express from "express";
import timeseries from "./timeseries"
import Library from "./library";
import * as fs from 'fs';
import csv from 'csvtojson';
import parse from 'csv-parse';

const router = express.Router();

router.get('/:file', (req, res) => {
	const file = Library.getPath('Timeseries', req.params.file + '.csv');
	if (fs.existsSync(file)) {
		var csvData = [];
		fs.createReadStream(file)
			.pipe(parse({ delimiter: ',' }))
			.on('data', function(csvrow) {
				csvData.push(csvrow);
			})
			.on('end', function() {
				res.set('Content-Type', 'application/json');
				res.send(csvData);
			});
		console.log("sending file " + file);
	} else {
		res.sendStatus(404);
	}
});

export default router;