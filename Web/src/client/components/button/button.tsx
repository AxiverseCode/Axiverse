import * as React from 'react';
import styles from './button.css';

export interface Props {
    value?: string;
}

class Button extends React.Component<Props> {
    render(): React.ReactNode {
        return (
            <button className={styles.button}>
                {this.props.value}
                {this.props.children}
            </button>
        );
    }
}

export default Button;