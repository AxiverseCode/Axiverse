import * as path from 'path'

const basePath = "D:/DataRoot"

export default class Library {
	static getPath(...paths) {
		return path.join.apply(null, [basePath].concat(Array.from(paths)));
	}
}

