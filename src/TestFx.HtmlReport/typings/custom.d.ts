interface JQuery {
  treeview(...args: any[]): Array<any>;
  antiscroll(...args: any[]): void;
  getContext(...args: any[]): void;
}

interface HTMLElement {
    getContext(string: string);
}

declare var Chart: any;