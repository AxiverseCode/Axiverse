import configJson from '../config.json';
import {Database, OPEN_CREATE, OPEN_READWRITE, verbose } from 'sqlite3';
import path from 'path';
import fs from 'fs';

verbose();

enum ColumnType {
	Null,
	Integer,
	Real,
	Text,
	Blob,
}

interface Column {
	name: string;
	type: ColumnType;
	isNullable: boolean;
	isPrimaryKey: boolean;
}

class Store {

	readonly databasePath: string;
	readonly databaseConnection: string;
	readonly database: Database;

	constructor() {
		this.databasePath = path.join(configJson['data-root'], 'Databases', 'Storage.sqlite');
		this.databaseConnection = Store.uriFromPath(this.databasePath);
		console.log(this.databasePath);
		if (!fs.existsSync(this.databasePath)) {
			console.log(`Database does not exist at ${this.databasePath}. Creating database.`);
			this.database = new Database(this.databasePath, OPEN_CREATE | OPEN_READWRITE, this.onLoaded.bind(this));
		} else {
			console.log(`Opening existing database at ${this.databasePath}.`);
			this.database = new Database(this.databasePath, OPEN_READWRITE, this.onLoaded.bind(this));
		}
	}

	private tableInfo(table: string, callback: (columns: Column[]) => void) {
		this.database.all('PRAGMA table_info($table)', {$table:table}, (err, rows) => {
			let columns = [];
			rows.forEach(element => {
				columns.push({
					name: rows['name'],
					type: ColumnType[rows['type']],
					isNullable: rows['notnull'] !== 0,
					isPrimaryKey: rows['pk'] === 1,
				});
			});
			callback(columns);
		});
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


interface Operation {
	operationId: string;
}

export class Operations {
	constructor(readonly database: Store) {

	}

	public ensure() {

	}

	public create(operation: Operation) {

	}

	public read(operationId: string) {

	}

	public update(partialOperation: Partial<Operation>) {

	}

	public delete(operationId: string) {

	}

	public query() {

	}
}

export class Timeseries {

}

export class Knowledge {

}

console.log('Loading');
const store = new Store();
const database = {
	store: store,
	operations: new Operations(store),
};

export default database;