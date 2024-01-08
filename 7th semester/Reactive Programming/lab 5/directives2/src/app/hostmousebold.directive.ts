import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';
@Directive({
    selector: '[hostmousebold]',
    host: {
        '(mouseenter)': 'onMouseEnter()',
        '(mouseleave)': 'onMouseLeave()'
    }
})
export class HostMouseBoldDirective {
    constructor(private element: ElementRef, private renderer: Renderer2) {
        this.renderer.setStyle(this.element.nativeElement, "cursor", "pointer");
    }
    @Input("inputSize") selectedSize = "25px";
    private defaultSize = "16px";
    onMouseEnter() {
        this.setFontWeight("bold");
        this.setColor("red");
        this.setSize(this.selectedSize);
    }
    onMouseLeave() {
        this.setFontWeight("normal");
        this.setColor("black");
        this.setSize(this.defaultSize);
    }
    private setFontWeight(val: string) {
        this.renderer.setStyle(this.element.nativeElement, "font-weight", val);
    }
    private setColor(val: string) {
        this.renderer.setStyle(this.element.nativeElement, "color", val);
    }
    private setSize(val: string) {
        this.renderer.setStyle(this.element.nativeElement, "fontSize", val);
    }
}