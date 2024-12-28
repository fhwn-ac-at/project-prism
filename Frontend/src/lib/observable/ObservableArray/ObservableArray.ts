import { Observer, Subject, Subscription } from "rxjs";
import { PushedEvent } from "./events/PushedEvent";
import { PoppedEvent } from "./events/PoppedEvent";
import { Event } from "./events/Event";
import { ClearEvent } from "./events/ClearEvent";
import { SetEvent } from "./events/SetEvent";

export class ObservableArray<T>
{
    private event: Subject<Event> = new Subject<Event>();

    private items: T[];

    public constructor(items: T[])
    {
        this.items = items;
    }

    public GetItems(): T[]
    {
        return this.items;
    }

    public Set(items: T[])
    {
        this.items = items;

        this.event.next(new SetEvent<T>(this.items));
    }

    public Push(item: T)
    {
        this.items.push(item);

        this.event.next(new PushedEvent(item));
    }

    public Pop(): T | undefined
    {
        const poppedItem = this.items.pop();

        this.event.next(new PoppedEvent<T>(poppedItem));

        return poppedItem;
    }

    public SubscribeEvent(obs: Partial<Observer<Event>>): Subscription
    {
        return this.event.subscribe(obs);
    }

    public Clear(): void
    {
        this.items = [];
        this.event.next(new ClearEvent())
    }
}