import { Directive, HostListener, Input, HostBinding } from '@angular/core';
@Directive({
    selector: '[inputSize]'
})
export class InputDirective {

    @Input("inputSize") selectedSize = "18px";
    @Input() defaultSize2 = "16px";

    private fontSize: string;
    private fontWeight = "normal";
    constructor() {
        this.fontSize = this.defaultSize2;
    }
    @HostBinding("style.fontSize") get getFontSize() {

        return this.fontSize;
    }
    @HostBinding("style.fontWeight") get getFontWeight() {

        return this.fontWeight;
    }
    @HostBinding("style.cursor") get getCursor() {
        return "pointer";
    }
    @HostListener("mouseenter") onMouseEnter() {
        this.fontWeight = "bold";
        this.fontSize = this.selectedSize;
    }
    @HostListener("mouseleave") onMouseLeave() {
        this.fontWeight = "normal";
        this.fontSize = this.defaultSize2;
    }
}
