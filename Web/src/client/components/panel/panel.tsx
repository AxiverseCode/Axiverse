import * as React from 'react';
import styles from './panel.css';

class Panel extends React.Component {
    render(): React.ReactNode {
        return (
            <div className={styles.panel}>
                {this.props.children}
            </div>
        );
    }
}

export default Panel;