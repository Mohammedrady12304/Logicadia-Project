import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'xpToLevel',
})
export class XpToLevelPipe implements PipeTransform {
  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }
}
