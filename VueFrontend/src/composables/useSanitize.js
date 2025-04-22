export function useSanitize() {
    function removeEmptyArraysAndNulls(obj) {
      if (Array.isArray(obj)) {
        return obj
          .map(removeEmptyArraysAndNulls)
          .filter(item => item !== undefined);
      }
  
      if (typeof obj === 'object' && obj !== null) {
        const newObj = {};
        for (const key in obj) {
          const val = obj[key];
  
          // ❌ Boş array ya da null ise atla
          if ((Array.isArray(val) && val.length === 0) || val === null) {
            continue;
          }
  
          const cleaned = removeEmptyArraysAndNulls(val);
  
          if (cleaned !== undefined) {
            newObj[key] = cleaned;
          }
        }
  
        return Object.keys(newObj).length > 0 ? newObj : undefined;
      }
  
      return obj;
    }
  
    return {
      removeEmptyArraysAndNulls,
    };
  }
  