import * as React from 'react';
import classNames from 'classnames';

import styles from './table.css';

export interface Props {
    className?: any;
}

export default class Table extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        
    };

    public render(): React.ReactNode {
        let className = classNames(styles.this, this.props.className);
        return (
            <table className={className}>
                <thead>
                    <tr>
                        <th>Asset Name</th>
                        <th>Class</th>
                        <th className='n'>Quantity</th>
                        <th className='n'>Value</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Orbiter</td>
                        <td>Satellite</td>
                        <td className='n'>2</td>
                        <td className='n'>¤736,343,231.03</td>
                    </tr>
                    <tr>
                        <td>Voyager</td>
                        <td>Passenger</td>
                        <td className='n'>1</td>
                        <td className='n'>¤19,373,931,212.93</td>
                    </tr>
                    <tr>
                        <td>Discovery</td>
                        <td>Cargo</td>
                        <td className='n'>4</td>
                        <td className='n'>¤124,634,234.85</td>
                    </tr>
                </tbody>
                {this.props.children}
            </table>
        );
    }
}