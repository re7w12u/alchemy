import { Pipe, PipeTransform } from '@angular/core';


declare function escape(s: string): string; 

@Pipe({
  name: 'recipeDecode'
})
export class RecipeDecodePipe implements PipeTransform {

  transform(value: any, args?: any): any {
      return decodeURIComponent(escape(value));
  }

}
