import * as React from 'react';
import * as d3 from 'd3';
import classNames from 'classnames';

import styles from './svgcomponent.css';

export interface Props {
    className?: any;
}

export default class SvgComponent extends React.Component<Props> {
    public static defaultProps: Partial<Props> = {
        
    };

    data: [number, number][];
    ref: React.RefObject<HTMLDivElement> = React.createRef(); 

    svg: d3.Selection<d3.BaseType, {}, null, undefined>;
    html: d3.Selection<d3.BaseType, {}, null, undefined>;

    public render(): React.ReactNode {
        let className = classNames(styles.this, this.props.className);
        return (
            <div className={className} ref={this.ref}></div>
        );
    }

    xScale: d3.ScaleLinear<number, number>;
    yScale: d3.ScaleLinear<number, number>;

    componentDidMount() {
		let element = this.ref.current;
		let props = this.props;

        this.data =
            [3, 6, 2, 7, 5, 2, 1, 3, 8, 9, 2, 5, 9, 3, 6, 3, 6, 2, 7, 5, 2, 1, 3, 8, 9, 2, 5, 9, 2, 7, 5, 2, 1, 3, 8, 9, 2, 5, 9, 3, 6, 2, 7, 5, 2, 1, 3, 8, 9, 2, 9]
            .map((v, i): [number, number] => [i, v]);

		for (var i = 1; i < this.data.length; i++) {
			this.data[i][1] = this.data[i - 1][1] + Math.random() * 2 - 1;
		}

		// reference to svg element containing circles
		this.svg = d3.select(element)
			.append('svg')
			.attr("width", "100%")
			.attr("height", "100%");

		// reference to html element containing text
		this.html = d3.select(element).append('div')
			.attr('class', 'bubble-chart-text');

		var size = {
			width: element.offsetWidth,
			height: element.offsetHeight
		}

		this.xScale = d3.scaleLinear()
			.domain([0, this.data.length])
			.range([3, size.width - 3]);

		this.yScale = d3.scaleLinear()
			.domain([-5, 15])
			.range([0, size.height]);

		this.svg.append("svg:circle")
			.attr("cx", this.xScale(this.data.length - 1) + 0.5)
			.attr("cy", this.yScale(this.data[ this.data.length - 1][1]))
			.attr("r", 2);

		this.componentDidUpdate();
    }

    componentDidUpdate() {	
        let element = this.ref.current;
		var data = this.data;

		if (!data) return;

		// assign new data to existing DOM for circles and labels
		var line = d3.line()
			.x((d, i) => this.xScale(i))
            .y((d, i) => this.yScale(d[1]))
            .curve(d3.curveMonotoneX);
			//.interpolate("monotone");

		this.svg.insert("svg:path", ":first-child").attr("d", line(data));

    }

    componentWillUnmount() {

    }
}