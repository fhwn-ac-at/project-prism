export interface Config
{
    canvasOptions: 
    {
        strokeWidth: number,
        strokeColor: string,
    }
}

export function isConfig(obj: any): obj is Config 
{
    return  typeof obj === 'object' &&
            obj !== null &&
            typeof obj.canvasOptions === 'object' &&
            typeof obj.canvasOptions.strokeWidth === 'number' &&
            typeof obj.canvasOptions.strokeColor === 'string'
}
