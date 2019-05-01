import configJson from './config.json';
import {Database, OPEN_CREATE, OPEN_READWRITE, verbose } from 'sqlite3';
import path from 'path';
import fs from 'fs';

verbose();

class Storage {

	readonly databasePath: string;
	readonly databaseConnection: string;
	readonly database: Database;

	constructor() {
		this.databasePath = path.join(configJson['data-root'], 'Databases', 'Storage.sqlite');
		this.databaseConnection = Storage.uriFromPath(this.databasePath);
		console.log(this.databasePath);
		if (!fs.existsSync(this.databasePath)) {
			console.log(`Database does not exist at ${this.databasePath}. Creating database.`);
			this.database = new Database(this.databasePath, OPEN_CREATE | OPEN_READWRITE, this.onLoaded.bind(this));
		} else {
			console.log(`Opening existing database at ${this.databasePath}.`);
			this.database = new Database(this.databasePath, OPEN_READWRITE, this.onLoaded.bind(this));
		}
	}

	private onLoaded(err) {
		console.log('Database loaded.');
		this.database.serialize(() => {
			this.database.all("SELECT name FROM sqlite_master WHERE type='table'", (err, rows) => console.log(rows));
			this.database.all("PRAGMA table_info(lorem)", (err, rows) => console.log(rows));
			console.log('Writing values.');
			//this.database.run("CREATE TABLE lorem (info TEXT)", (err) => console.log(err));
			console.log('Writing values.');
			this.database.run("INSERT INTO lorem VALUES ('Hello')", (err) => console.log(err));
			console.log('Writing values.');
			//this.database.close((err) => console.log('Database closed.'));
		});
	}

	public static uriFromPath(str : string) : string {
	    var pathName = path.resolve(str).replace(/\\/g, '/');

	    // Windows drive letter must be prefixed with a slash
	    if (pathName[0] !== '/') {
	        pathName = '/' + pathName;
	    }

	    return encodeURI('file://' + pathName);
	};
}

export default Storage;