import * as React from 'react';
import styles from './input.css';

export interface Props {
    value?: string;
    placeholder?: string;
    type?: 'text' | 'password';
}

class Input extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        type: 'text',
    };

    public render(): React.ReactNode {
        return (
            <input
                className={styles.input}
                type={this.props.type}
                placeholder={this.props.placeholder}>
            </input>
        );
    }
}

export default Input;