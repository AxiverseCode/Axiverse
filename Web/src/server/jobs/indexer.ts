import path from 'path';
import fs from 'fs';

interface Dataset {
	datasetId: string;

}

class Indexer {
	constructor(readonly root:string) {
		// code...
	}

	public crawl() :void {
		// look for _dataset.json
		// compare last update time with time in database
		// If there is a difference, perform an update
	}

	public create() {
		
	}

	public update() {

	}
}