
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';


/*  will import app module ==> that call  app component  and all different libraries */ 
platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
